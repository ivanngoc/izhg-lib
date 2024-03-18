using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziHardGames.Apps.Abstractions.Lib.Tasks
{
    [IziTask]
    internal interface IIziTask
    {

    }

    /// <summary>
    /// пемечает то, что должно выполниться. Это что-то назовем задачей (Task) по аналогии с TPL. 
    /// Задача это единица выполнения, которая может быть выполняться синхронно, асинхронно, паралельно или парально асинхронно.
    /// Задача выполняет роль свзяующего звена между задачами для их планирования, синхронизации или выстраивания цепочек выполнения.<br/>
    /// Этот атрибут создан в дополнение к интерфейсу так как readonly ref struct не может иметь интерфейсы
    /// </summary>
    public class IziTaskAttribute : Attribute
    {

    }
}
