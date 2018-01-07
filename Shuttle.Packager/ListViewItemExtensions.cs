using System.Windows.Forms;

namespace Shuttle.Packager
{
    public static class ListViewItemExtensions
    {
        internal static Package Package(this ListViewItem item)
        {
            return (Package) item.Tag;
        }
    }
}