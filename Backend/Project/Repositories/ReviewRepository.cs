using Project.Context;

namespace Project.Repositories;

public class ReviewRepository
{
    private readonly MasterContext _context;

    public ReviewRepository(MasterContext masterContext)
    {
        _context = masterContext;
    }

   

}