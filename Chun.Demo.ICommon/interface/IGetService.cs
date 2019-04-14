using System;

namespace Chun.Demo.ICommon
{
    public interface IGetService
    {
        event Action OnCompleted;
        void GetService(PhraseHtmlType phraseHtmlType);
    }
}
