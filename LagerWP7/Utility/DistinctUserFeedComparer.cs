using System;
using System.Collections.Generic;
using UntappdAPI.DataContracts;

namespace LagerWP7 {
    class DistinctUserFeedComparer : IEqualityComparer<UserFeedResults> {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(UserFeedResults x, UserFeedResults y) {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y))
                return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x.User, null) || Object.ReferenceEquals(y.User, null))
                return false;

            return x.User.UserName == y.User.UserName;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(UserFeedResults result) {
            //Check whether the object is null
            if (Object.ReferenceEquals(result, null))
                return 0;

            if (Object.ReferenceEquals(result.User, null))
                return 0;

            //Get hash code for the Name field if it is not null.
            return result.User.UserName.GetHashCode();
        }
    }
}
