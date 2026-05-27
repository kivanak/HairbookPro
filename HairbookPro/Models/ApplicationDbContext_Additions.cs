// OVO NE DODAJ KAO POSEBAN CLASS FILE AKO VEC IMAS ApplicationDbContext.
// Otvori Models/IdentityModels.cs i unutar klase ApplicationDbContext dodaj ove DbSet-ove:

public System.Data.Entity.DbSet<HairBookPro.Models.ServiceCategory> ServiceCategories { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.Service> Services { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.Stylist> Stylists { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.Appointment> Appointments { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.BlogCategory> BlogCategories { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.BlogPost> BlogPosts { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.Comment> Comments { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.Review> Reviews { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.GalleryImage> GalleryImages { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.ContactMessage> ContactMessages { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.AppointmentLog> AppointmentLogs { get; set; }
public System.Data.Entity.DbSet<HairBookPro.Models.DeletedCommentLog> DeletedCommentLogs { get; set; }
