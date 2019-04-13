using System;
using System.Collections.Generic;
using System.Text;

namespace mmx
{
    public interface IAudioRecorder
    {
        void Start();

        string Stop();
    }
}
