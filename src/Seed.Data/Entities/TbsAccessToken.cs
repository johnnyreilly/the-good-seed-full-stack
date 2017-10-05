using System;
using System.ComponentModel.DataAnnotations;

namespace Seed.Data.Entities
{
    public class TbsAccessToken
    {
        // Properties
        [Key]
        public Guid Id { get; private set; }

        [Required, MaxLength(1024)]
        public string AccessToken { get; private set; }

        [Required, MaxLength(50)]
        public string UserId { get; private set; }

        public int? LegalEntityId { get; private set; }

        [Required]
        public DateTime Expiry { get; private set; }

        [Required]
        public DateTime CreatedOn { get; private set; }


        // C'tors
        private TbsAccessToken()
        { }

        public TbsAccessToken(string accessToken, string userId, int? legalEntityId, DateTime expiry)
        {
            this.Id = Guid.NewGuid();
            this.AccessToken = accessToken;
            this.UserId = userId;
            this.LegalEntityId = legalEntityId;
            this.Expiry = expiry;
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}