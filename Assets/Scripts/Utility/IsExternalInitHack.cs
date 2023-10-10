// Hack required to use C# 9's init
// Shoutouts to Unity for not implementing all of C# 9 and having this problem in the first place
// Sources: https://forum.unity.com/threads/unity-future-net-development-status.1092205/page-10#post-7693588
// Unity rep approves: https://forum.unity.com/threads/unity-future-net-development-status.1092205/page-11#post-7698016
namespace System.Runtime.CompilerServices
{
    class IsExternalInit { }
}