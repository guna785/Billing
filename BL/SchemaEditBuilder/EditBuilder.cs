using BL.BLService;
using BL.SchemaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SchemaEditBuilder
{
    public class EditBuilder
    {
        IStocksRepository _stock;
        IUserRepository _user;
        ICategoryRepository _category;
        IProductTypeRepository _productType;
        IProductCompanyRepository _company;
        ISuplierRepository _suplier;
        IClientRepostory _client;
        IInvoiceRepository _invoice;
        IPymentRepositroy _pyment;
        ILogRepository _logs;
        ICompanyProfileRepository _companyProfile;
        public EditBuilder(IStocksRepository stock, IUserRepository user, ICategoryRepository category, IProductTypeRepository productType,
                               IProductCompanyRepository company, ISuplierRepository suplier, IClientRepostory client, IInvoiceRepository invoice,
                                IPymentRepositroy pyment, ILogRepository logs, ICompanyProfileRepository companyProfile)
        {
            _stock = stock;
            _user = user;
            _category = category;
            _productType = productType;
            _company = company;
            _suplier = suplier;
            _client = client;
            _invoice = invoice;
            _pyment = pyment;
            _logs = logs;
            _companyProfile = companyProfile;
        }

        public async Task<T> ReturnObjectData<T>(string id)
        {
            var obj = typeof(T).Name;
            if (obj.Equals("EditStock"))
            {
                var obdata = await _stock.GetStockID(id);
                return (T)Convert.ChangeType(new EditStock()
                {
                    actprice = obdata.actprice,
                    cmname = obdata.cmname,
                    company = obdata.company,
                    discount = obdata.discount,
                    minalert = obdata.minalert,
                    mrp = obdata.mrp,
                    name = obdata.name,
                    qty = obdata.qty,
                    tax = obdata.tax,
                    warranty = obdata.warranty,
                    Id = obdata.Id

                }, typeof(T));
            }
            else if (obj.Equals("EditCategory"))
            {
                var obdata = await _category.GetCatagoryID(id);
                return (T)Convert.ChangeType(new EditCategory()
                {
                    name = obdata.name,

                    Id = obdata.Id

                }, typeof(T));
            }
            else if (obj.Equals("EditProductType"))
            {
                var obdata = await _productType.GetProductTypeID(id);
                return (T)Convert.ChangeType(new EditProductType()
                {
                    name = obdata.name,

                    Id = obdata.Id

                }, typeof(T));
            }
            else if (obj.Equals("EditPCompany"))
            {
                var obdata = await _company.GetProductCompanyID(id);
                return (T)Convert.ChangeType(new EditPCompany()
                {
                    name = obdata.name,

                    Id = obdata.Id

                }, typeof(T));
            }
            else if (obj.Equals("EditEmployee"))
            {
                var obdata = await _user.GetUserID(id);
                return (T)Convert.ChangeType(new EditEmployee()
                {
                    name = obdata.name,
                    uname = obdata.uname,
                    address = obdata.address,
                    dob = obdata.dob.ToString(),
                    email = obdata.email,
                    joindate = obdata.joindate.ToString(),
                    phone = obdata.phone,
                    remarks = obdata.remarks,
                    role = obdata.role,
                    Id = obdata.Id
                }, typeof(T));
            }
            else if (obj.Equals("EditClient"))
            {
                var obdata = await _client.GetClientID(id);
                return (T)Convert.ChangeType(new EditClient()
                {
                    name = obdata.name,
                    Id = obdata.Id,
                    address = obdata.address,
                    email = obdata.email,
                    gender = obdata.gender,
                    gst = obdata.gst,
                    pan = obdata.pan,
                    phone = obdata.state,
                    state = obdata.state
                }, typeof(T));
            }
            else if (obj.Equals("EditSuplier"))
            {
                var obdata = await _suplier.GetSuplierID(id);
                return (T)Convert.ChangeType(new EditSuplier()
                {
                    name = obdata.name,
                    Id = obdata.Id,
                    pan = obdata.pan,
                    gst = obdata.gst,
                    email = obdata.email,
                    address = obdata.address,
                    cphone = obdata.cphone,
                    cpname = obdata.cpname
                }, typeof(T));
            }
            else if (obj.Equals("PayInvoice"))
            {
                var obdata = await _invoice.GetInvoiceID(id);
                return (T)Convert.ChangeType(new PayInvoice()
                {
                    amount = obdata.balance,
                    Id = obdata.Id

                }, typeof(T));
            }
            else if (obj.Equals("CompanyProfileEdit"))
            {
                var obdata = await _companyProfile.GetCompanyProfile();
                var o = obdata.FirstOrDefault();
                if (o != null)
                {
                    return (T)Convert.ChangeType(new CompanyProfileEdit()
                    {
                        name = o.name,
                        address = o.address,
                        bankaccno = o.bankaccno,
                        bankbranch = o.bankbranch,
                        bankname = o.bankname,
                        email = o.email,
                        gst = o.gst,
                        hsn = o.hsn,
                        ifsc = o.ifsc,
                        pan = o.pan,
                        phone = o.phone,
                        state = o.state,
                        tin = o.tin,
                        web = o.web,
                        Id = o.Id

                    }, typeof(T));
                }
            }
            return (T)Convert.ChangeType(null, typeof(T));
        }
    }
}
