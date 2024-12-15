
using CollageSystem.Core.Models;
using CollageSystem.Utilities.Helpers.CustomAttributes;

namespace CollageSystem.Utilities.Helpers.CustomAttributes;

public class UniquePhoneNumberAttribute() : UniqueAttribute<Person>("PhoneNumber");