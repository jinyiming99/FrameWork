using System;


public class SpanBuff
{
    public char[] splitArray;
    
    private int _length;
    public int Length => _length;
    public void Check(int length)
    {
        if (splitArray == null || splitArray.Length < length + _length)
            splitArray = new char[(length + _length) * 5];
    }
    public void Reset()
    {
        _length = 0;
    }

    public void Write(ReadOnlySpan<char> span)
    {
        var s = splitArray.AsSpan(_length, span.Length);
        span.CopyTo(s);
        _length += span.Length;
    }
    public void Write(char c)
    {
        splitArray[_length++] = c;
    }
    
    public ReadOnlySpan<char> AsSpan()
    {
        return splitArray.AsSpan(0, _length);
    }
}

public static class StringSplitExtensions
{
    public static ReadOnlySplitEnumerator SplitReadOnlySpan(this string str, char[] separator)
    {
        return new ReadOnlySplitEnumerator(str.AsSpan(), separator);
    }

    public static SpanBuff splitArray = new SpanBuff();
    public static SplitEnumerator SplitSpan(this string str, char[] separator)
    {
        splitArray.Reset();
        splitArray.Check(str.Length);
        
        //result = str.ToCharArray();
        return new SplitEnumerator(str.AsSpan(), separator,splitArray);
    }
}

public ref struct ReadOnlySplitEntry
{
    public ReadOnlySpan<char> first { get; private set; }
    public ReadOnlySplitEntry(ReadOnlySpan<char> first)
    {
        this.first = first;
    }
    public static implicit operator ReadOnlySpan<char>(ReadOnlySplitEntry entry) => entry.first;
}

public ref struct SplitEntry
{
    public ReadOnlySpan<char> first { get; private set; }
    public char mid { get; private set; }
    public bool isEnd;
    public SpanBuff _separator;
    public SplitEntry(ReadOnlySpan<char> first,char mid,SpanBuff span, bool isEnd)
    {
        this.first = first;
        this.mid = mid;
        this.isEnd = isEnd;
        _separator = span;
    }

    public void Write()
    {
        _separator.Write(first);
        _separator.Write(mid);
    }
    
    public void Write(ReadOnlySpan<char> separator)
    {
        _separator.Write(separator);
        _separator.Write(mid);
    }
    public string ToString()
    {
        return string.Create<SpanBuff>(_separator.Length, _separator, (span, entry) =>
        {
            var s = entry.AsSpan();
            for (int i = 0; i < entry.Length; i++)
            {
                span[i] = s[i];
            }
        });
    }
    
    public static implicit operator ReadOnlySpan<char>(SplitEntry entry) => entry.first;
}

public ref struct ReadOnlySplitEnumerator
{
    private ReadOnlySpan<char> _str;
    private ReadOnlySpan<char> _separator;
    public ReadOnlySplitEntry Current { get; private set; }
    
    public ReadOnlySplitEnumerator GetEnumerator() => this;
    public ReadOnlySplitEnumerator(ReadOnlySpan<char> str, char[] separator)
    {
        _str = str;
        _separator = separator;
        Current = default;
    }
    
    public bool MoveNext()
    {
        var s = _str;
        if (s.Length == 0)
            return false;
        var index = _str.IndexOf(_separator);
        if (index == -1)
        {
            _str = ReadOnlySpan<char>.Empty;
            Current = new ReadOnlySplitEntry(s);
            return true;
        }
        Current = new ReadOnlySplitEntry(s.Slice(0, index));
        _str = s.Slice(index + 1);
        return true;
    }
}
public ref struct SplitEnumerator
{
    private ReadOnlySpan<char> _str;
    private SpanBuff _result;
    private ReadOnlySpan<char> _separator;
    public SplitEntry Current { get; private set; }
    
    public SplitEnumerator GetEnumerator() => this;
    public SplitEnumerator(ReadOnlySpan<char> str, ReadOnlySpan<char> separator,SpanBuff result)
    {
        _str = str;
        _separator = separator;
        _result = result;
        Current = default;
    }
    
    public bool MoveNext()
    {
        var s = _str;
        if (s.Length == 0)
            return false;
        int index = FindIndex(s,out var outChar);
        //var index = _str.IndexOf(_separator);
        if (index == -1)
        {
            _str = Span<char>.Empty;
            Current = new SplitEntry(s,(char)0,_result,true);
            return true;
        }
        _str = s.Slice(index + 1);
        Current = new SplitEntry(s.Slice(0, index), outChar,_result,_str.Length == 0);

        return true;
    }
    
    private int FindIndex(ReadOnlySpan<char> span,out char outChar)
    {
        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];
            for (int j = 0; j < _separator.Length; j++)
            {
                if (_separator[j] == c)
                {
                    outChar = c;
                    return i;
                }
            }
        }
        outChar = ' ';
        return -1;
    }
}
