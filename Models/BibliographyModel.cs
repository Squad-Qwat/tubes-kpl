using System.Data;
using System.Linq;
namespace PaperNest_API.Models
{
    public class BibliographyModel
    {
        private DataTable citationsTable;

        public BibliographyModel()
        {
            citationsTable = new DataTable("Citations");
            citationsTable.Columns.Add("Id", typeof(int));
            citationsTable.Columns.Add("Author", typeof(string));
            citationsTable.Columns.Add("Title", typeof(string));
            citationsTable.Columns.Add("Publication", typeof(string));
            citationsTable.Columns.Add("Year", typeof(int));
            citationsTable.Columns.Add("Page", typeof(string));
            citationsTable.PrimaryKey = new DataColumn[] { citationsTable.Columns["Id"] };
        }

        public DataTable GetCitations()
        {
            return citationsTable;
        }

        public Citation GetCitationById(int id)
        {
            DataRow row = citationsTable.Rows.Find(id);
            if (row != null)
            {
                return DataRowToCitation(row);
            }
            return null;
        }

        public void AddCitation(Citation citation)
        {
            DataRow newRow = citationsTable.NewRow();
            newRow["Id"] = citation.Id;
            newRow["Author"] = citation.Author;
            newRow["Title"] = citation.Title;
            newRow["Publication"] = citation.Publication;
            newRow["Year"] = citation.Year;
            newRow["Page"] = citation.Page;
            citationsTable.Rows.Add(newRow);
        }

        public void UpdateCitation(Citation citation)
        {
            DataRow row = citationsTable.Rows.Find(citation.Id);
            if (row != null)
            {
                row["Author"] = citation.Author;
                row["Title"] = citation.Title;
                row["Publication"] = citation.Publication;
                row["Year"] = citation.Year;
                row["Page"] = citation.Page;
            }
        }

        public void DeleteCitation(int id)
        {
            DataRow row = citationsTable.Rows.Find(id);
            if (row != null)
            {
                citationsTable.Rows.Remove(row);
            }
        }

        private Citation DataRowToCitation(DataRow row)
        {
            return new Citation
            {
                Id = (int)row["Id"],
                Author = (string)row["Author"],
                Title = (string)row["Title"],
                Publication = (string)row["Publication"],
                Year = (int)row["Year"],
                Page = (string)row["Page"]
            };
        }
    }
}