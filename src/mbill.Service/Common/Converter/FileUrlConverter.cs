namespace mbill.Service.Common.Converter;

public class FileUrlConverter : IValueConverter<FileEntity, string>
{
    private readonly IFileRepo _fileRepo;

    public FileUrlConverter(IFileRepo fileRepo)
    {
        _fileRepo = fileRepo;
    }
    public string Convert(FileEntity sourceMember, ResolutionContext context)
    {
        if (sourceMember == null) return "";
        return _fileRepo.GetFileUrl(sourceMember.Path);
    }
}


public class StringUrlConverter : IValueConverter<string, string>
{
    private readonly IFileRepo _fileRepo;

    public StringUrlConverter(IFileRepo fileRepo)
    {
        _fileRepo = fileRepo;
    }
    public string Convert(string sourceMember, ResolutionContext context)
    {
        if (sourceMember.IsNullOrWhiteSpace()) return "";
        return _fileRepo.GetFileUrl(sourceMember);
    }
}

