using System;
using System.Buffers.Binary;
using GameFrameWork.DebugTools;
using UnityEngine.Rendering;

namespace GameFrameWork.Network
{
    public class DataSegment
    {
        
        private const int DefLength = 1024;
                
        private static bool IsLittleEndian = true;
        
        public byte[] m_data;

        public int m_writePos = 0;
        public int m_readPos = 0;

        public void ClearPos()
        {
            m_writePos = 0;
        }

        public DataSegment()
        {
            m_data = new byte[DefLength];
        }
        
        public DataSegment(int size)
        {
            m_data = new byte[size];
        }

        public int Length
        {
            get => m_writePos;
            set => m_writePos = value;
        }

        public int Pos
        {
            get => m_readPos;
            set => m_readPos = value;
        }

        public void MoveData()
        {
            int index = 0;
            for (int i = m_readPos; i < m_writePos; i++)
            {
                m_data[index++] = m_data[i];
            }
            m_writePos -= m_readPos;
            m_readPos = 0;
            
        }
        

        public void ResetSize(int length)
        {
            if (m_data.Length < length)
            {
                int l = DefLength;
                while (true)
                {
                    l *= 2;
                    if (l >= length)
                    {
                        var newData = new byte[l];
                        if (m_writePos > 0)
                        {
                            System.Buffer.BlockCopy(m_data, 0, newData, 0, m_writePos);
                        }
                        m_data = newData;
                        return;
                    }
                }
            }
        }
        public void Write(ulong data)
        {
            ResetSize(m_writePos + 8);
            Span<byte> span = m_data;
            var b = IsLittleEndian | BitConverter.IsLittleEndian;
            if (b)
            {
                for (int i = 0; i < 8; i++)
                {
                    span[m_writePos + i] = (byte)((data >> (i * 8)) & 0xFF);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    span[m_writePos + i] = (byte)((data >> ((7 - i) * 8)) & 0xFF);
                }
            }
            

            m_writePos += 8;
        }
        public void Write(int data)
        {
            ResetSize(m_writePos + 4);
            if (IsLittleEndian)
            {
                m_data[m_writePos] = (byte)((data >> 24) & 0xFF);
                m_data[m_writePos + 1] = (byte)((data >> 16) & 0xFF);
                m_data[m_writePos + 2] = (byte)((data >> 8) & 0xFF);
                m_data[m_writePos + 3] = (byte)(data & 0xFF);
            }
            else
            {
                m_data[m_writePos] = (byte)(data & 0xFF);
                m_data[m_writePos + 1] = (byte)((data >> 8) & 0xFF);
                m_data[m_writePos + 2] = (byte)((data >> 16) & 0xFF);
                m_data[m_writePos + 3] = (byte)((data >> 24) & 0xFF);
            }
            m_writePos += 4;
        }

        public void Write(short data)
        {
            ResetSize(m_writePos + 2);
            if (IsLittleEndian)
            {
                m_data[m_writePos] = (byte)((data >> 8) & 0xFF);
                m_data[m_writePos + 1] = (byte)(data & 0xFF);
            }
            else
            {
                m_data[m_writePos] = (byte)(data & 0xFF);
                m_data[m_writePos + 1] = (byte)((data >> 8) & 0xFF);
            }
            m_writePos += 2;
        }

        public void Write(byte data)
        {
            ResetSize(m_writePos + 1);
            m_data[m_writePos] = (byte)(data & 0xFF);
            m_writePos += 1;
        }
        
        public void WriteArray(byte[] data, int length)
        {
            if (length > 0)
            {
                ResetSize(m_writePos + length);
                System.Buffer.BlockCopy(data, 0, m_data, m_writePos, length);
                m_writePos += length;
            }
        }

        public void Write(byte[] data)
        {
            if (data.Length > 0)
            {
                if (m_data.Length < m_writePos + data.Length)
                    ResetSize(m_writePos + data.Length);
                try
                {
                    System.Buffer.BlockCopy(data, 0, m_data, m_writePos, data.Length);
                }
                catch (Exception e)
                {
                    DebugHelper.LogError(e.ToString());
                    return;
                }
                m_writePos += data.Length;
            }
        }

        public void Write(byte[] data, int length)
        {
            if (length >= 0)
            {
                Write(length);
                ResetSize(m_writePos + length);
                System.Buffer.BlockCopy(data, 0, m_data, m_writePos, length);
                m_writePos += length;
            }
        }

        public bool TryReadLong(out long o)
        {
            var b = (m_readPos + 8) <= Length;
            if (b)
            {
                TryReadInt(out int n1);
                TryReadUInt(out uint n2);
                o = ((long) n1) << 32 | ((long) n2);
                m_readPos += 8;
            }
            else
            {
                o = 0;
            }
            return b;
        }
        
        public bool TryReadInt(out int o)
        {
            var b = (m_readPos + 4) <= Length;
            if (b)
            {
                if (IsLittleEndian)
                {
                    o = (int)(m_data[m_readPos + 3] |
                              (int)(m_data[m_readPos + 2] << 8) |
                              (int)(m_data[m_readPos + 1] << 16) |
                              (int)(m_data[m_readPos] << 24));
                }
                else
                {
                    o = (int)(m_data[m_readPos] |
                              (int)(m_data[m_readPos + 1] << 8) |
                              (int)(m_data[m_readPos + 2] << 16) |
                              (int)(m_data[m_readPos + 3] << 24));
                              
                }
                m_readPos += 4;
            }
            else
            {
                o = 0;
            }
            return b;
        }
        
        public bool TryReadUInt(out uint o)
        {
            var b = (m_readPos + 4) <= Length;
            if (b)
            {
                o = (uint) (m_data[m_readPos + 3] |
                            (uint) (m_data[m_readPos + 2] << 8) |
                            (uint) (m_data[m_readPos + 1] << 16) |
                            (uint) (m_data[m_readPos] << 24));
                m_readPos += 4;
            }
            else
            {
                o = 0;
            }
            return b;
        }
        
        public bool TryReadByte(out byte o)
        {
            var b = (m_readPos + 1) <= Length;
            if (b)
            {
                o = m_data[m_readPos];
                m_readPos++;
            }
            else
            {
                o = default;
            }
            
            return b;
        }
        public bool TryReadShort(out short o)
        {
            var b = (m_readPos + 2) <= Length;
            if (b)
            {
                if (IsLittleEndian)
                    o = (short) (m_data[m_readPos + 1] | (m_data[m_readPos] << 8));
                else
                {
                    o = (short) (m_data[m_readPos] | (m_data[m_readPos + 1] << 8));
                }
                m_readPos += 2;
            }
            else o = default;
            
            return b;
        }

        public bool TryReadDatas(int l,out byte[] data)
        {
            var b = (m_readPos + l) <= Length;
            if (b)
            {
                data = new byte[l];
                System.Buffer.BlockCopy(m_data, m_readPos,data, 0, l);
                m_readPos += l;
            }
            else
            {
                data = null;
            }

            return b;
        }
        
        public bool TryReadDatas(out byte[] data,out int length)
        {
            if (TryReadInt(out length))
            {
                if (TryReadDatas(length, out data))
                {
                    return true;
                }
            }

            data = null;
            return false;
        }
    }
}