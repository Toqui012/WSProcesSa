using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Models;

namespace WSProcesSa.Classes
{
	public class Policies
	{
		public const string Admin = "Dennisse";
		public const string Funcionario = "Funcionario";
		public const string Diseñador_de_Proceso = "Diseñador de Proceso";
		public const string Any = "Any";

		public static AuthorizationPolicy AdminPolicy()
		{
			return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
		}

		public static AuthorizationPolicy AnyPolicy()
		{
			return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(new string[] { Admin, Funcionario, Diseñador_de_Proceso }).Build();
		}
	}
}
