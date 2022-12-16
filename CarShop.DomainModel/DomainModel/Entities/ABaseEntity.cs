namespace CarShop.DomainModel.Entities;

public abstract class ABaseEntity {

   #region properties
   public abstract Guid Id {get; set;}
   #endregion

   #region ctor
   protected ABaseEntity() {}
   #endregion

   #region methods   
   #endregion
}