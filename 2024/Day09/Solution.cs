using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2024.Day09 {

    [ProblemName("Disk Fragmenter")]
    class Solution : Solver {

        private record struct Block(int Id, int Length);
        
        public object PartOne(string input) =>
            Sum(Compact(ParseBlocks(input), fragmentsEnabled: true));

        public object PartTwo(string input) =>
            Sum(Compact(ParseBlocks(input), fragmentsEnabled: false));

        private static LinkedList<Block> ParseBlocks(string input) {
            LinkedList<Block> list = new LinkedList<Block>();

            int index = 0;
            foreach (char ch in input) {
                int length = ch - '0';
                int id = (index % 2 == 1) ? -1 : (index / 2);

                list.AddLast(new Block(id, length));
                index++;
            }

            return list;
        }

        private static LinkedList<Block> Compact(LinkedList<Block> list, bool fragmentsEnabled) {
            LinkedListNode<Block> i = list.First;
            LinkedListNode<Block> j = list.Last;

            while (i != j) {
                if (i != null && i.Value.Id != -1) {
                    i = i.Next;
                } else if (j != null && j.Value.Id == -1) {
                    j = j.Previous;
                } else
                {
                    Move(list, i, j, fragmentsEnabled);
                    j = j?.Previous;
                }
            }

            return list;
        }


        private static void Move(LinkedList<Block> list, LinkedListNode<Block> start, LinkedListNode<Block> source, bool fragmentsEnabled) {
            for (LinkedListNode<Block> i = start; i != source; i = i?.Next) {

                if ((i != null && i.Value.Id != -1) ||
                    i == null)
                    continue;
                
                int freeLen = i.Value.Length;
                int blockLen = source.Value.Length;
                
                if (freeLen == blockLen) {
                    (i.Value, source.Value) = (source.Value, i.Value);
                    return;
                }
                
                if (freeLen > blockLen) {
                    int leftover = freeLen - blockLen;

                    i.Value = source.Value;
                    source.Value = new Block(-1, blockLen);

                    list.AddAfter(i, new Block(-1, leftover));
                    return;
                }
                
                if (freeLen < blockLen && fragmentsEnabled) {
                    int remainder = blockLen - freeLen;

                    i.Value = new Block(source.Value.Id, freeLen);
                    source.Value = new Block(source.Value.Id, remainder);

                    list.AddAfter(source, new Block(-1, freeLen));
                }
            }
        }


        private static long Sum(LinkedList<Block> list) {
            long result = 0;
            int pos = 0;

            for (LinkedListNode<Block> node = list.First; node != null; node = node.Next) {
                int id = node.Value.Id;
                int length = node.Value.Length;

                if (id != -1) {
                    for (int k = 0; k < length; k++) {
                        result += (long)pos * id;
                        pos++;
                    }
                } else {
                    pos += length;
                }
            }

            return result;
        }
    }
}
