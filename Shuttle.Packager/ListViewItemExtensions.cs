using System.Windows.Forms;

namespace Shuttle.Packager
{
    public static class ListViewItemExtensions
    {
        internal static Package GetPackage(this ListViewItem item)
        {
            return (item.Tag as Package) ?? throw new ApplicationException($"Could not get the 'Package' for list view item '{item.Text}'.");
        }
    }
}