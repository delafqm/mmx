﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace mmx
{
    public interface IAudioRecorder
    {
        void Start(string filepath);

        void Stop();

        void Play(string filepath, Button btn, string name);
    }
}
