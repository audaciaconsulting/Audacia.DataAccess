namespace Audacia.DataAccess.Model;

public interface IRowVersionable
{
    byte[] RowVersion { get; set; }
}