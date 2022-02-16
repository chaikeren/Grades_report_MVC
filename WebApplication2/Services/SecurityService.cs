using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class SecurityService
    {
        UserDAO userDAO = new UserDAO();

        StudentsSubjectsAverageDAO studentsDao = new StudentsSubjectsAverageDAO();


        public List<UserSubjetctsAverageModel> IsValid(UserModel user)
        {
           return userDAO.FindUserByIdAndPassword(user);
        }

        public List<StudentsSubjectsAverageModel> GetSubjects()
        {
            return studentsDao.ShowAllSubjects();
        }
    }
}
