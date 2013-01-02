using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Forms;
using StripTease.Bouncer;
using StripTease.Dancers;

namespace StripTease.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
           
            _tags = new List<string>();
            _selectedTags = new List<string>();
            _removeTagsVisibility = Visibility.Hidden;
       
         
        }
        #region Fields

        private List<string> _fileList;
        private List<string> _fileExt; 
        private List<string> _tags;
        private List<string> _fullPathList; 
        private readonly List<string> _selectedTags;
        private Visibility _removeTagsVisibility;
        private ICommand _openFolder;
        private ICommand _removeTags;
        private ICommand _selectedTag;
        private ICommand _selectAll;
        private string _selectedFolder;
        private string _selectedExtension;
        private string _selectedFile;
        private string _selectedFileText;
        private bool _selectAllTags;
        

        #endregion Fields

        #region Public Commands
        /// <summary>
        /// Gets the select all.
        /// </summary>
        public ICommand SelectAll
        {
            get { return _selectAll ?? (_selectAll = new RelayCommand(SelectAllMethod)); }
        }
        /// <summary>
        /// Gets the selected tag.
        /// </summary>
        public ICommand SelectedTag
        {
            get { return _selectedTag ?? (_selectedTag = new RelayCommand<string>(SelectedTagMethod)); }
        }
        /// <summary>
        /// Gets the remove tags.
        /// </summary>
        public ICommand RemoveTags
        {
            get { return _removeTags ?? (_removeTags = new RelayCommand(RemoveTagsMethod)); }
        }

        /// <summary>
        /// Gets the open folder.
        /// </summary>
        public ICommand OpenFolder
        {
            get { return _openFolder ?? (_openFolder = new RelayCommand(OpenFolderMethod)); }
        }
        #endregion Public Commands
        #region Public Values
        /// <summary>
        /// Gets a value indicating whether [select all tags].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [select all tags]; otherwise, <c>false</c>.
        /// </value>
        public bool SelectAllTags
        {
            get
            {
                return _selectAllTags;
            }
           
        }
        /// <summary>
        /// Gets or sets the remove tags visibility.
        /// </summary>
        /// <value>
        /// The remove tags visibility.
        /// </value>
        public Visibility RemoveTagsVisibility
        {
            get { return _removeTagsVisibility; }
            set { _removeTagsVisibility = value;
            RaisePropertyChanged("RemoveTagsVisibility");
            }
        }
        /// <summary>
        /// Gets the tags.
        /// </summary>
        public List<string> Tags
        {
            get
            {
                if(_tags != null)
                {
                    return _tags;
                }
                return _tags;
            }
            
        }
        /// <summary>
        /// Gets the extension list.
        /// </summary>
        public List<string> ExtensionList
        {
            get
            {
                if (_fileExt != null)
                {
                    return _fileExt;
                }
                return _fileExt;
            }
        }
        /// <summary>
        /// Gets the file list.
        /// </summary>
        public List<string> FileList
        {
            get
            {
                if(_fileList != null)
                {
                    return _fileList;
                }
                return _fileList;
            }
        }
        /// <summary>
        /// Gets the selected folder.
        /// </summary>
        public string SelectedFolder
        {
            get { return _selectedFolder; }
        }
        /// <summary>
        /// Gets or sets the selected extension.
        /// </summary>
        /// <value>
        /// The selected extension.
        /// </value>
        public string SelectedExtension
        {
            get { return _selectedExtension; }
            set
            {
                _selectedExtension = value;
                GetFilteredFiles(_selectedExtension);
                RaisePropertyChanged("SelectedExtension");
            }
        }
        /// <summary>
        /// Gets or sets the selected file.
        /// </summary>
        /// <value>
        /// The selected file.
        /// </value>
        public string SelectedFile
        {
            get
            {
                return _selectedFile;
            }
            set
            {
                _selectedFile = value;
                ShowSelectedFileMethod(value);
                RaisePropertyChanged("SelectedFile");
            }
        }
        /// <summary>
        /// Gets or sets the selected file text.
        /// </summary>
        /// <value>
        /// The selected file text.
        /// </value>
        public string SelectedFileText
        {
            get { return _selectedFileText; }
            set { _selectedFileText = value;
            RaisePropertyChanged("SelectedFileText");
            }
        }
        #endregion Public Values


        #region Private Methods
        /// <summary>
        /// Opens the folder method.
        /// </summary>
        private void OpenFolderMethod()
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
           if(result == DialogResult.OK)
           {
               _selectedFolder = dialog.SelectedPath;
               GetFileList(dialog.SelectedPath);
               _removeTagsVisibility = Visibility.Hidden;
               RaisePropertyChanged("SelectedFolder");
               RaisePropertyChanged("RemoveTagsVisibility");
           }
        }

        /// <summary>
        /// Gets the file list.
        /// </summary>
        /// <param name="path">The path.</param>
        private void GetFileList(string path)
        {
            _fileList = new List<string>();
            _fileExt = new List<string>();
            foreach (var file in Directory.GetFiles(path))
            {
                var fileExtension = Path.GetExtension(file);
                if(!_fileExt.Contains(fileExtension))
                {
                    _fileExt.Add(fileExtension);
                }
                if (!_fileList.Contains(file))
                {

                    _fileList.Add(Path.GetFileName(file));
                }
            }
            RaisePropertyChanged("ExtensionList");
            RaisePropertyChanged("FileList");
        }
        /// <summary>
        /// Gets the filtered files.
        /// </summary>
        /// <param name="filter">The filter.</param>
        private void GetFilteredFiles(string filter)
        {
            _fileList = new List<string>();
            _fullPathList = new List<string>();
            foreach (var file in Directory.GetFiles(_selectedFolder,"*"+filter))
            {
                
                if (!_fileList.Contains(file))
                {
                    _fullPathList.Add(file);
                    _fileList.Add(Path.GetFileName(file));
                }
            }
            GetTags();
   
            RaisePropertyChanged("FileList");
        }
        /// <summary>
        /// Gets the tags.
        /// </summary>
        private void GetTags()
        {

            var tagFinder = new TagFinder();
            _tags = tagFinder.Tags(_fullPathList);
            _removeTagsVisibility = Visibility.Visible;
            RaisePropertyChanged("RemoveTagsVisibility");
            RaisePropertyChanged("Tags");
        }

        /// <summary>
        /// Removes the tags method.
        /// </summary>
        private void RemoveTagsMethod()
        {
            if (_selectedTags.Count > 0)
            {
                var shad = new Shad();
                shad.Bounce(_selectedTags, _fullPathList);
            }
            else
            {
                System.Windows.MessageBox.Show("Select a tag first");
            }
            System.Windows.MessageBox.Show("Complete");
           GetTags();
            _selectedFileText = "";
            _selectedTags.Clear();
           RaisePropertyChanged("SelectedFileText");
            RaisePropertyChanged("Tags");
        }
        /// <summary>
        /// Selecteds the tag method.
        /// </summary>
        /// <param name="tag">The tag.</param>
        private void SelectedTagMethod(string tag)
        {
            if(_selectedTags.Contains(tag))
            {
                _selectedTags.Remove(tag);
            }
            else
            {
                _selectedTags.Add(tag);
            }
           
        }
        /// <summary>
        /// Shows the selected file method.
        /// </summary>
        /// <param name="file">The file.</param>
        private void ShowSelectedFileMethod(string file)
        {
            try
            {
               
                var fullPath = _selectedFolder + "\\" + file;
                using (var streamReader = new StreamReader(fullPath))
                {
                    _selectedFileText = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                
                
             
                RaisePropertyChanged("SelectedFileText");
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
                
               
            }
         
        }
        /// <summary>
        /// Selects all method.
        /// </summary>
        private void SelectAllMethod()
        {
            if(_selectAllTags)
            {
                _selectAllTags = false;
                _selectedTags.Clear();
                RaisePropertyChanged("SelectAllTags");
                return;
            }
            if(!_selectAllTags)
            {
                _selectAllTags = true;
                _selectedTags.Clear();
                _selectedTags.AddRange(_tags);
                RaisePropertyChanged("SelectAllTags"); 
                
            }
          
        }
        #endregion Private Methods
    }
}