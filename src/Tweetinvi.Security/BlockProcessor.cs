// File copyrighted by Mono - https://github.com/danielcrenna/hammock/blob/master/src/netCF/Hammock.Compact/Mono/Security/Cryptography/BlockProcessor.cs

using System;

namespace Tweetinvi.Security
{
    public class BlockProcessor
    {
        private readonly ICryptoTransform _transform;
        private readonly byte[] _block;
        private readonly int _blockSize;
        private int _blockCount;

        public BlockProcessor(ICryptoTransform transform)
            : this(transform, transform.InputBlockSize)
        {
        }

        public BlockProcessor(ICryptoTransform transform, int blockSize)
        {
            _transform = transform;
            _blockSize = blockSize;
            _block = new byte[blockSize];
        }

        ~BlockProcessor()
        {
            Array.Clear(_block, 0, _blockSize);
        }

        public void Initialize()
        {
            Array.Clear(_block, 0, _blockSize);
            _blockCount = 0;
        }

        public void Core(byte[] rgb)
        {
            Core(rgb, 0, rgb.Length);
        }

        public void Core(byte[] rgb, int ib, int cb)
        {
            int count = Math.Min(_blockSize - _blockCount, cb);
            Buffer.BlockCopy(rgb, ib, _block, _blockCount, count);
            _blockCount += count;
            if (_blockCount == _blockSize)
            {
                _transform.TransformBlock(_block, 0, _blockSize, _block, 0);
                int num = (cb - count) / _blockSize;
                for (int index = 0; index < num; ++index)
                {
                    _transform.TransformBlock(rgb, count + ib, _blockSize, _block, 0);
                    count += _blockSize;
                }

                _blockCount = cb - count;
                if (_blockCount > 0)
                {
                    Buffer.BlockCopy(rgb, count + ib, _block, 0, _blockCount);
                }
            }
        }

        public byte[] Final()
        {
            return _transform.TransformFinalBlock(_block, 0, _blockCount);
        }
    }
}
