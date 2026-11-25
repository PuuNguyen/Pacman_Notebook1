using System;
using System.Collections.Generic;
using System.IO;

namespace WinFormsApp1
{
    public class Map
    {
        public char[,] Grid { get; }
        public int Rows => Grid.GetLength(0);
        public int Columns => Grid.GetLength(1);

        public Map(string levelData)
        {
            if (string.IsNullOrWhiteSpace(levelData))
                throw new ArgumentException("Bản đồ trống.", nameof(levelData));

            var lines = new List<string>();
            using (var reader = new StringReader(levelData))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line.TrimEnd());
            }

            // Bỏ các dòng trống ở cuối file (không bỏ dòng trống ở giữa)
            while (lines.Count > 0 && lines[^1].Length == 0)
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                throw new ArgumentException("Bản đồ không có dữ liệu.", nameof(levelData));

            int columns = 0;
            foreach (var l in lines)
                columns = Math.Max(columns, l.Length);

            Grid = new char[lines.Count, columns];
            for (int row = 0; row < lines.Count; row++)
                for (int col = 0; col < columns; col++)
                    Grid[row, col] = col < lines[row].Length ? lines[row][col] : '#';
        }

        public static Map FromFile(string path) => new Map(File.ReadAllText(path));
    }
}