using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.ParticleEditor
{
	public interface IController
	{
		Game.Particles.ParticlesData ParticleData { get; }

		void RefreshEffectsList();
	}
}
