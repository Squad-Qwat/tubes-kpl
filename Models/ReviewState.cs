namespace PaperNest_API.Models
{
    /*
     * 'override' keyword hanya digunakan untuk superclass (termasuk abstract class) di C#, contoh:
        public abstract class ReviewState
        {
            public abstract string Name { get; }
            public abstract void Process(ResearchRequest request, ReviewResult result, string reviewerComment);
        }
        Pada interface, kita tidak bisa menggunakan keyword 'override' karena interface tidak memiliki implementasi.
    */
    public interface ReviewState
    {
        string Name { get; }
        void Process(ResearchRequest request, ReviewResult result, string reviewerComment);
    }

    public class SubmittedState : ReviewState
    {
        public string Name => "Submitted";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            if (result != ReviewResult.Pending)
            {
                Console.WriteLine($"Error: Tidak bisa secara langsung diubah ke {result} dari {Name}. Perlu ditinjau terlebih dahulu.");
            }
        }
    }
    public class UnderReviewState : ReviewState
    {
        public string Name => "Under Review";

        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {   
            request.AddReview(new Review(Guid.NewGuid(), request.Id, "Reviewer", result, reviewerComment));

            switch (result)
            {
                case ReviewResult.Approved:
                    request.ChangeState(new ApprovedState());
                    break;
                case ReviewResult.Rejected:
                    request.ChangeState(new RejectedState());
                    break;
                case ReviewResult.NeedsRevision:
                    request.ChangeState(new NeedsRevisionState());
                    break;
                default:
                    Console.WriteLine("Hasil tinjauan masih dipertimbangkan.");
                    break;
            }
        }
    }
    public class ApprovedState : ReviewState
    {
        public string Name => "Approved";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            Console.WriteLine("Permintaan peninjauan telah disetujui.");
        }
    }

    public class RejectedState : ReviewState
    {
        public string Name => "Rejected";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            Console.WriteLine("Permintaan peninjauan telah ditolak.");
        }
    }

    public class NeedsRevisionState : ReviewState
    {
        public string Name => "Needs Revision";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {   
            if (result == ReviewResult.Approved)
            {
                request.ChangeState(new ApprovedState());
            } 
            else
            {
                Console.WriteLine($"Permintaan peninjauan masih direvisi atau sudah menerima hasil tinjauan lain: {result}");
            }
        }
    }
}