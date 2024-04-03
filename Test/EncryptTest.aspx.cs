using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project.Donna.Test
{
    public partial class EncryptTest : Common.Common
    {
        public string strKey = ConfigurationManager.AppSettings["brdKey"];
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            string strEncrypt = txtPlain.Text;

            try
            {
                //파라미터 암호화
                strEncrypt = EncryptString(strEncrypt + "," + strKey, strKey);

                lblEncrypt.Text = strEncrypt;
            }
            catch(Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            string strDecrypt = txtEncrypt.Text;
            try
            {
                strDecrypt = DecryptString(strDecrypt, strKey);

                lblDecrypt.Text = strDecrypt;
            }
            catch (Exception ex)
            {
                Common.Error.WebHandler(ex);
            }
        }
    }
}