﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalAutomationCommunity.DAO;

namespace MedicalAutomationCommunity.DBGateway
{
    public class StockDBGateway:DBGateway
    {
        public List<Stock> GetAll(int centerId)
        {
            string query = "SELECT *FROM tbl_stock where centerId=" + centerId;
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlDataReader = ASqlCommand.ExecuteReader();

            List<Stock> stockList = new List<Stock>();

            while (ASqlDataReader.Read())
            {
                MedicineDBGateway aGateway = new MedicineDBGateway(); ;
                Stock aStock = new Stock();
                Medicine aMedicine = aGateway.Find(Convert.ToInt32(ASqlDataReader["medicineId"]));
                aStock.MedicineName = aMedicine.Name + "_" + aMedicine.Quantity;
                aStock.Quantity = (int)ASqlDataReader["quantity"];

                stockList.Add(aStock);
            }
            ASqlDataReader.Close();
            ASqlConnection.Close();

            return stockList;
        }

        public void AddInventoryDetails(Stock aStock)
        {
            string query = "INSERT INTO tbl_stock VALUES('" + aStock.DistrictId + "','" + aStock.ThanaId + "','" + aStock.CenterId + "','" + aStock.MedicineId + "','" + aStock.Quantity + "')";
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlCommand.ExecuteNonQuery();
            ASqlConnection.Close();
        }

        public void Update(int centerId, int medicineId, int quantity)
        {
            string query = "Update tbl_stock Set quantity-='" + quantity + "' where centerid='" + centerId + "' AND medicineId='" + medicineId + "'";
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlCommand.ExecuteNonQuery();
            ASqlConnection.Close();
        }
    }
}
