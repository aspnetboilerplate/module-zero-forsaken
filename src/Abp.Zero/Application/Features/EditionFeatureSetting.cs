using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Editions;

namespace Abp.Application.Features
{
    /// <summary>
    /// Feature setting for an <see cref="Edition"/>.
    /// </summary>
    public class EditionFeatureSetting : FeatureSetting
    {
        [ForeignKey("EditionId")]
        public virtual Edition Edition { get; set; }
        public virtual int EditionId { get; set; }

        public EditionFeatureSetting()
        {
            
        }

        public EditionFeatureSetting(int editionId, string name, string value)
            :base(name, value)
        {
            EditionId = editionId;
        }
    }
}