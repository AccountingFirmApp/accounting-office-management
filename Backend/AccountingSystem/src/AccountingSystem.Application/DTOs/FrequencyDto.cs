namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for Frequency - תדירויות דיווח (חודשי, רבעוני, שנתי)
/// </summary>
public class FrequencyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO ליצירת תדירות חדשה
/// </summary>
public class CreateFrequencyDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// DTO לעדכון תדירות
/// </summary>
public class UpdateFrequencyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
