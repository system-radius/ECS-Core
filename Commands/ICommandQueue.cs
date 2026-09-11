using ECS.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Core.Commands
{
    internal interface ICommandQueue
    {
        void Playback(ComponentRegistry components);
        void Clear();
    }
}
