using System;

namespace Traction.Tests {

    [Flags]
    internal enum ContractTypes {
        None = 0,
        Pre = 1,
        Post = 2,
        PreAndPost = Pre | Post
    }
}
