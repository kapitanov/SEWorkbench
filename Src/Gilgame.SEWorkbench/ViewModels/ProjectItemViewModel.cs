﻿using System;
using System.Linq;
using Gilgame.SEWorkbench.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Gilgame.SEWorkbench.Services;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectItemViewModel : INotifyPropertyChanged
    {
        private Services.ObservableSortedList<ProjectItemViewModel> _Children;
        public Services.ObservableSortedList<ProjectItemViewModel> Children
        {
            get
            {
                return _Children;
            }
        }

        private ProjectItemViewModel _Parent;
        public ProjectItemViewModel Parent
        {
            get
            {
                return _Parent;
            }
        }

        private ProjectItem _Model;
        public ProjectItem Model
        {
            get
            {
                return _Model;
            }
        }

        public string Name
        {
            get
            {
                return _Model.Name;
            }
        }
        public ProjectItemType Type
        {
            get
            {
                return _Model.Type;
            }
        }
        public string Path
        {
            get
            {
                return _Model.Path;
            }
        }

        public string Blueprint
        {
            get
            {
                return _Model.Blueprint;
            }
        }

        private ObservableSortedList<GridItemViewModel> _Grid;
        public ObservableSortedList<GridItemViewModel> Grid
        {
            get
            {
                return _Grid;
            }
        }

        private bool _IsExpanded = false;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (value != _IsExpanded)
                {
                    _IsExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                if (_IsExpanded && _Parent != null)
                {
                    _Parent.IsExpanded = true;
                }
            }
        }

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                if (value != _IsVisible)
                {
                    _IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }

                if (_IsVisible && _Parent != null)
                {
                    _Parent.IsVisible = true;
                }
                if (!_IsVisible && _Parent != null)
                {
                    _Parent.IsVisible = false;
                }
            }
        }

        private bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                    _Model.Project.RaiseSelectionChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public ProjectItemViewModel(ProjectItem item) : this(item, null)
        {
            _FileRequestedCommand = new Commands.FileRequestedCommand(this);
        }

        public ProjectItemViewModel(ProjectItem item, ProjectItemViewModel parent)
        {
            _Model = item;
            _Parent = parent;

            _Children = new Services.ObservableSortedList<ProjectItemViewModel>(
                (from child in _Model.Children select new ProjectItemViewModel(child, this)).ToList<ProjectItemViewModel>(),
                new Comparers.ProjectItemComparer<ProjectItemViewModel>()
            );

            _Grid = new Services.ObservableSortedList<GridItemViewModel>(
                new GridItemViewModel[] { },
                new Comparers.GridItemComparer<GridItemViewModel>()
            );
        }

        public void AddChild(ProjectItem item)
        {
            _Children.Add(new ProjectItemViewModel(item, this));
            Model.Children.Add(item);
        }

        public void AddChild(ProjectItem item, Interop.Grid grid)
        {
            ProjectItemViewModel vm = new ProjectItemViewModel(item, this);
            vm.Grid.Add(CreateGridViewModel(grid));

            _Children.Add(vm);
            Model.Children.Add(item);
        }

        public void Remove()
        {
            _Parent.RemoveChild(this);
        }

        private void RemoveChild(ProjectItemViewModel child)
        {
            Model.Children.Remove(child.Model);
            _Children.Remove(child);
        }

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(Name))
            {
                return false;
            }
            return Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        public void SetGrid(Interop.Grid grid)
        {
            GridItemViewModel vm = CreateGridViewModel(grid);

            _Grid.Clear();
            _Grid.Add(vm);
        }

        private GridItemViewModel CreateGridViewModel(Interop.Grid grid)
        {
            GridItemViewModel root = new GridItemViewModel(new GridItem() { Name = grid.Name });
            foreach(KeyValuePair<string, List<Interop.TerminalBlock>> pair in grid.Blocks)
            {
                GridItemViewModel node = new GridItemViewModel(new GridItem() { Name = pair.Key });
                foreach(Interop.TerminalBlock block in pair.Value)
                {
                    node.AddChild(new GridItemViewModel(new GridItem() { Name = block.Name }));
                }
                root.AddChild(node);
            }
            return root;
        }

        #region File Requested Command

        private readonly ICommand _FileRequestedCommand;
        public ICommand FileRequestedCommand
        {
            get
            {
                return _FileRequestedCommand;
            }
        }

        public void RaiseFileRequested()
        {
            _Model.Project.RaiseFileRequested();
        }

        #endregion
    }
}
