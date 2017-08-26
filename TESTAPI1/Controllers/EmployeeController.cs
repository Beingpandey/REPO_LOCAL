using EmployeeDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TESTAPI1.Controllers
{
    public class EmployeeController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBModel entities = new EmployeeDBModel())
            {               
                return entities.Employees.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (EmployeeDBModel entities = new EmployeeDBModel())
                {
                    var entity = entities.Employees.Where(x => x.ID == id).FirstOrDefault();
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id =" + id.ToString() + " was not found.");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBModel entities = new EmployeeDBModel())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.OK, employee);
                    message.Headers.Location = new Uri(Request.RequestUri +"/"+ employee.ID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                return message;
            }
            
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBModel entities = new EmployeeDBModel())
                {
                    var entity = entities.Employees.SingleOrDefault(x => x.ID == id);
                    if(entity==null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee With ID = " + id.ToString() + " Doesn't Exist..");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Employee With ID = " + id.ToString()+" Deleted Successfully");
                    }
                }
            }
            catch(Exception ex)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                return message;
            }
        }

        public HttpResponseMessage Put(int id,[FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBModel entities = new EmployeeDBModel())
                {
                    var entity = entities.Employees.FirstOrDefault(x=>x.ID==id);
                    if(entity==null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound,"Employee With ID= "+id.ToString()+" Not Found..");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, employee);
                    }
                }

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
