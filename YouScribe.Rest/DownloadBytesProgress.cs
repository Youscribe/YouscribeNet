﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public class DownloadBytesProgress
    {
        public DownloadBytesProgress(string fileName, int bytesReceived, int totalBytes)
        {
            Filename = fileName;
            BytesReceived = bytesReceived;
            TotalBytes = totalBytes;
        }

        public int TotalBytes { get; private set; }

        public int BytesReceived { get; private set; }

        public float PercentComplete { get { return (float)BytesReceived / TotalBytes; } }

        public string Filename { get; private set; }

        public bool IsFinished { get { return BytesReceived == TotalBytes; } }
    }
}
