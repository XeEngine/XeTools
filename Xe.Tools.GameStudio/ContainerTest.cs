using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.GameStudio
{
    public class ContainerTest : List<Project.Item>
    {
        public Project.Container Container { get; set; }

        public ContainerTest()
        {
            Container = new Project.Container();
            Container.Name = "my container";
            Container.Items = new List<Project.Item>()
            {
                new Project.Item()
                {
                    Type = "image",
                    Alias = "my alias",
                    Input = "my input",
                    Output = "my output",
                    Parameters = new Project.Parameter[]
                    {
                        new Project.Parameter()
                        {
                            Name = "parameter name",
                            Value = "parameter value"
                        }
                    }
                },
                new Project.Item()
                {
                    Type = "image",
                    Input = "$(InputDir)/images/dummy.png",
                    Output = "$(OutputDir)/images/dummy.png",
                },
                new Project.Item()
                {
                    Type = "image",
                    Input = "$(InputDir)/images/sprites/test.png",
                    Output = "$(OutputDir)/images/sprites/test.png",
                },
                new Project.Item()
                {
                    Type = "audio",
                    Input = "$(InputDir)/sound/se/tone.wav",
                    Output = "$(OutputDir)/sound/se/tone.wav",
                },
                new Project.Item()
                {
                    Type = "message",
                    Input = "$(InputDir)/log.json",
                    Output = "$(OutputDir)/log.msg",
                }
            };

            foreach (var item in Container.Items)
                Add(item);

        }
    }
}
