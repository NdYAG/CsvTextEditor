﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System;
    using System.Linq;

    public static class StringExtensions
    {
        public static string InsertCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount)
        {
            var newLine = Environment.NewLine;

            if (column == columnsCount)
            {
                return text.Replace(newLine, Symbols.Comma + newLine) + Symbols.Comma;
            }

            if (column == 0)
            {
                return text.Insert(0, Symbols.Comma.ToString())
                    .Replace(newLine, newLine + Symbols.Comma);
            }

            var newCount = text.Length + linesCount;
            var textArray = new char[newCount];
            var indexer = 0;

            var commaCounter = 1;
            foreach (var c in text)
            {
                if (c == Symbols.Comma)
                {
                    if (commaCounter == column)
                    {
                        textArray[indexer] = Symbols.Comma;
                        indexer++;
                    }

                    commaCounter++;

                    if (commaCounter == columnsCount)
                    {
                        commaCounter = 1;
                    }
                }

                textArray[indexer] = c;
                indexer++;
            }

            return new string(textArray).TrimEnd(newLine);
        }

        public static string InsertLineWithTextTransfer(this string text, int insertLineIndex, int offsetInLine, int columnCount)
        {
            var newLine = Environment.NewLine;
            var newLineLenght = newLine.Length;

            if (offsetInLine == 0 || insertLineIndex == 0)
            {
                return InsertLine(text, insertLineIndex, columnCount);
            }

            var previousLineOffset = insertLineIndex == 1 ? 0 : text.IndexOfSpecificOccurance(newLine, insertLineIndex - 1) + newLineLenght;
            var leftLineChunk = text.Substring(previousLineOffset, offsetInLine);
            var splitColumnIndex = leftLineChunk.Count(x => x.Equals(Symbols.Comma));

            var insetionText = $"{new string(Symbols.Comma, columnCount - splitColumnIndex - 1)}{newLine}{new string(Symbols.Comma, splitColumnIndex)}";

            var insertPosition = previousLineOffset + offsetInLine;
            return text.Insert(insertPosition, insetionText).TrimEnd(newLine);
        }

        public static string InsertLine(this string text, int insertLineIndex, int columnsCount)
        {
            var newLine = Environment.NewLine;
            var newLineLenght = newLine.Length;

            var insertLineText = $"{new string(Symbols.Comma, columnsCount - 1)}{newLine}";
            var insertionPosition = insertLineIndex != 0 ? text.IndexOfSpecificOccurance(newLine, insertLineIndex) + newLineLenght : 0;

            return text.Insert(insertionPosition, insertLineText).TrimEnd(newLine);
        }

        public static string DuplicateTextInLine(this string csvText, int startOffset, int endOffset)
        {
            var newLine = Environment.NewLine;

            var lineToDuplicate = csvText.Substring(startOffset, endOffset - startOffset);
            if (!lineToDuplicate.EndsWith(newLine))
            {
                lineToDuplicate = newLine + lineToDuplicate;
            }

            return csvText.Insert(endOffset, lineToDuplicate).TrimEnd(newLine);
        }

        public static string RemoveText(this string csvText, int startOffset, int endOffset)
        {
            var newLine = Environment.NewLine;

            return csvText.Remove(startOffset, endOffset - startOffset).TrimEnd(newLine);
        }

        public static string RemoveCommaSeparatedColumn(this string text, int column, int linesCount, int columnsCount)
        {
            if (columnsCount == 0 || linesCount == 0)
            {
                return string.Empty;
            }

            var newLine = Environment.NewLine;
            var newLineLenght = newLine.Length;

            var newCount = text.Length;
            var textArray = new char[newCount];

            var commaCounter = 0;
            var indexer = 0;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == Symbols.Comma)
                {
                    commaCounter++;
                }

                if (IsLookupMatch(text, i, newLine))
                {
                    commaCounter = 0;

                    i += newLineLenght - 1;

                    foreach (var newLineChar in newLine)
                    {
                        textArray[indexer] = newLineChar;
                        indexer++;
                    }

                    continue;
                }

                if (commaCounter == column)
                {
                    continue;
                }

                textArray[indexer] = c;
                indexer++;
            }

            return new string(textArray, 0, indexer).TrimEnd(newLine); 
        }

        public static string TrimEnd(this string target, string trimString)
        {
            var result = target;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }

        private static bool IsLookupMatch(string text, int startIndex, string lookup)
        {
            var lookupLength = lookup.Length;
            if (text.Length - startIndex < lookupLength)
            {
                return false;
            }

            var lookupNewLine = text.Substring(startIndex, lookupLength);
            return string.Equals(lookupNewLine, lookup);
        }

        private static int IndexOfSpecificOccurance(this string source, string value, int occuranceNumber)
        {
            var index = -1;
            for (var i = 0; i < occuranceNumber; i++)
            {
                index = source.IndexOf(value, index + 1, StringComparison.Ordinal);

                if (index == -1)
                {
                    break;
                }
            }

            return index;
        }

    }
}