using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HT.Models
{
    public static class DbInitializer
    {
        public static void Initialize(HarmonyToysDatabaseContext context)
        {
            context.Database.EnsureCreated();

            if (context.TblUserDetails.Any())
            {
                return;
            }

            var users = new TblUserDetails[]
                {
                    new TblUserDetails {Fname="Test1", Lname="User1", Phone=7648756846, EmailId="malik@gmail.com", Address="delhi" },
                    new TblUserDetails {Fname="Nitin", Lname="User2", Phone=7677867567, EmailId="rohilla@gmail.com", Address="Gurgaon" },
                    new TblUserDetails {Fname="Test3", Lname="User3", Phone=8989898989, EmailId="ankush@test.com", Address="test" },
                    new TblUserDetails {Fname="Admin", Lname="admin", Phone=8898878778, EmailId="admin@admin.com", Address="Address1" }
                };

            context.TblUserDetails.AddRange(users);
            context.SaveChanges();

            var logins = new TblLoginDetails[]
            {
                new TblLoginDetails {UserName="malik@gmail.com", Password="1232",IsApproved="Y", IsAdmin="Y", UserId=1 },
                new TblLoginDetails {UserName="rohilla@gmail.com", Password="1212",IsApproved="N", IsAdmin="N", UserId=2 },
                new TblLoginDetails {UserName="ankush@test.com", Password="1231",IsApproved="N", IsAdmin="Y", UserId=3 },
                new TblLoginDetails {UserName="admin@admin.com", Password="admin",IsApproved="Y", IsAdmin="Y", UserId=4 }
            };

            context.TblLoginDetails.AddRange(logins);
            context.SaveChanges();
        }
        }
    }
