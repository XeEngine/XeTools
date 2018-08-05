using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Tools
{
    interface IInfoLastEdit
	{
		DateTime? GetInfoLastEdit();
		DateTime? GetInfoLastEditRecursive();
	}
}
