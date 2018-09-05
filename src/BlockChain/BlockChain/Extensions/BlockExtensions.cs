using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BlockChain.Extensions
{
    public static class BlockExtensions
    {
        public static byte[] GenerateHash(this IBlock block)
        {
            using (SHA512 sha = new SHA512Managed())
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(block.Data);
                writer.Write(block.Nonce);
                writer.Write(block.TimeStamp.ToBinary());
                writer.Write(block.PreviousHash);
                var streamArray = stream.ToArray();
                return sha.ComputeHash(streamArray);
            }
        }

        public static byte[] MineHash(this IBlock block, byte[] difficulty)
        {
            if (difficulty == null)
            {
                throw new ArgumentNullException(nameof(difficulty));
            }

            byte[] hash = new byte[0];
            int d = difficulty.Length;

            while (!hash.Take(d).SequenceEqual(difficulty) && block.Nonce <= int.MaxValue)
            {
                block.Nonce++;
                hash = block.GenerateHash();
            }
            return hash;
        }

        public static bool HasValidHash(this IBlock block)
        {
            var curr = block.GenerateHash();
            return block.Hash.SequenceEqual(curr);
        }

        public static bool HasValidPreviousHash(this IBlock block, IBlock previousblock)
        {
            if (previousblock == null)
            {
                throw new ArgumentException("previousblock is null", "previousblock");
            }
            var prev = previousblock.GenerateHash();
            return previousblock.HasValidHash() && block.PreviousHash.SequenceEqual(prev);
        }

        //public static List<String> MerkleTree(this IBlock block)
        //{
        //    List<String> tree = new List<string>();
        //    foreach (var t in block.Transactions)
        //    {
        //        //tree.Add(t.GetHashCode());
        //    }
        //    int levelOffset = 0;
        //    for (int levelSize = block.Transactions.Count; levelSize > 1; levelSize = (levelSize + 1) / 2)
        //    {
        //        for (int left = 0; left < levelSize; left += 2)
        //        {

        //            // The right hand node can be the same as the left hand, in the
        //            // case where we don't have enough
        //            // transactions.
        //            int right = Math.Min(left + 1, levelSize - 1);
        //            String tleft = tree[levelOffset + left];
        //            String tright = tree[levelOffset + right];
        //            tree.Add(GenerateHash(tleft + tright));
        //        }
        //        // Move to the next level.
        //        levelOffset += levelSize;
        //    }

        //    return tree;
        //}
    }
}
