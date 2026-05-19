using NetTopologySuite.Geometries;

namespace Modules.Trips.Domain.Entities
{
    /// <summary>
    ///  Query locations based on theme
    ///  Create planes via an algorithm that takes a time
    ///  return plans that satisfy the time quota 
    ///  give the places scores to decide how related they are to the plan
    ///  give the plans score based on the overall score of the places
    ///  let the rating contribute to the score
    ///  let the featured flag contribute to the score too
    ///  get the top X plans based on theme score
    ///  sort them based on rating and featured flag overall
    /// </summary>

    public class Governorate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<TripGovernorate> TripGovernorates { get; set; } = [];
    }
}