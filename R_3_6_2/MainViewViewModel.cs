using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_3_6_2
{
    public class MainViewViewModel
    {   // Свойства
        private ExternalCommandData _commandData;
        public List<FamilySymbol> Furniture { get; } = new List<FamilySymbol>();
        public FamilySymbol SelectedFurniture { get; set; }
        public List<Level> Level { get; } = new List<Level>();
        public Level SelectedLevel { get; set; }
        public DelegateCommand SaveCommand { get; }
        public List<XYZ> Points { get; set; } = new List<XYZ>();

        // Конструктор
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Furniture = FamilySymbolUtils.GetFamilySymbols(commandData);
            Level = LevelUtils.GetLavels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            Points = SelectionUtils.GetPoints(_commandData, "Выберете точку", ObjectSnapTypes.Points);
        }
        // Методы
        private void OnSaveCommand()
        {
            // обрращаемся к документу
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // создаю переменную точкки с нулевым индексом.
            // Переменная обращается к созданному классу,
            // прописывающему логику создания точки.
            XYZ insertpoint = Points[0];

            //Вызываю созданный класс FamilySymbolUtils для создания элемента семейства,
            //через него обращаюсь к методу этого класса CreateFamilyInstance для создания элемента семейства.
            //Передаю ему данные документа, Выбранное пользователем значение ComboBox SelectedFurniture
            //и только что созданную точку и выбранный уровень oLevel прописанный в классе. 
            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurniture, insertpoint, SelectedLevel);

            // Вызываю событие закрывающее окно приложения
            RaiseCloseRequest();

        }

        //событие создается для скрытие окна на время выбора.
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        //событие создается для повторного открытия окна после отработки программы.
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
        //событие создается для закрытия программы.
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }       

    }

}
