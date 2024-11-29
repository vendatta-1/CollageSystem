
using CollageSystem.Core.Models;
using CollageSystem.Utilities.Helpers.CustomAttributes;

namespace api.Helpers.CustomAttributes;

public class UniquePhoneNumberAttribute() : UniqueAttribute<Person>("PhoneNumber");