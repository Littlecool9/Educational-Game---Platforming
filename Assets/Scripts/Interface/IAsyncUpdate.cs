using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EducationalGame.Core
{
    public interface IAsyncUpdate
    {
        public Task UpdateAsync();
    }
}
