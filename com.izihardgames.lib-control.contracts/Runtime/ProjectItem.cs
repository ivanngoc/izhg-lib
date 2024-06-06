using System;

namespace IziHardGames.Libs.IziLibrary.Contracts
{
    public class ProjectItem
    {
        public Guid Guid { get; set; }
        public string Directory { get; set; } = string.Empty;
        /// <summary>
        /// Filename with extension
        /// </summary>
        public string FileName { get; set; } = string.Empty;
    }
}
