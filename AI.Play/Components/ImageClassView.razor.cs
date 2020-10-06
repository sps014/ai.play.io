using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace AI.Play.Components
{
    public partial class ImageClassView
    {
        [Parameter]
        public static List<ImageClassContainer> Classes { get; set; } = new List<ImageClassContainer>()
  {
    new ImageClassContainer(){ ClassName="A"},
    new ImageClassContainer(){ClassName="B"}
  };
        static int ClassNameCount = 0;
        void AddNewClass()
        {
            Classes.Add(new ImageClassContainer() { ClassName = "Class" + ClassNameCount++ });
            StateHasChanged();
            AddedDeletedCall?.Invoke();


        }
        void DeleteClass(int i)
        {
            Classes.RemoveAt(i);
            StateHasChanged();
            AddedDeletedCall?.Invoke();
        }

        public delegate void AddedDeletedCallHandler();
        public event AddedDeletedCallHandler AddedDeletedCall;
    }
}
