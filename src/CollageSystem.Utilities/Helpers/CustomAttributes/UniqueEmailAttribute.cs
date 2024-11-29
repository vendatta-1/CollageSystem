
using CollageSystem.Core.Models;

namespace CollageSystem.Utilities.Helpers.CustomAttributes;

public class UniqueEmailAttribute() : UniqueAttribute<Person>("Email");