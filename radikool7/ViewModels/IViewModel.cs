using System.Reactive.Disposables;
using System.Threading.Tasks;
using Radikool7.Models;

namespace Radikool7.ViewModels
{
    public interface IViewModel
    {
        void Init(MainModel model, CompositeDisposable disposable);
    }
}