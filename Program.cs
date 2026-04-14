using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace NorthwindSerializationApp
{
    //============================
    // DATA MODELS (CLASSES 
    //============================

    // Category class represents a prosuct category form the database
    [Serializable] // allows object to be serialized
    [DataContract] // used fro binary (DataContractSerializer)
public class Category
{
    [DataMember] // include in serialization
    public int CategoryID { get; set; }

    [DataMember]
    public string CategoryName { get; set; } = string.Empty;

    // One category can have many products
    [DataMember]
    public List<Product> Products { get; set; } = new();
}
// Product class represents individual products
[DataContract]
public class Product
{
    [DataMember]
    public int ProductID { get; set; }

    [DataMember]
    public string ProductName { get; set; } = string.Empty;
}
    class Program
    {
        static void Main()
        {   
            // =============================
            // DATABASE CONNECTION STRING
            // =============================
            // Connection TO SQL SERVER (your local machine)
            string connectionString = @"Server=LAPTOP-DU6SRIJM;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;";

            try
            {
                // 1. Get data from database
                var categories = GetCategoriesWithProducts(connectionString);

                // 2. Serialize data into different formats
                var results = new List<(string Format, int Size)>
                {
                    ("JSON", SerializeToJson(categories)),
                    ("XML", SerializeToXml(categories)),
                    ("Binary", SerializeToBinary(categories))
                };

                // STEP 3: Sort results from smallest to largest
                var ranked = results.OrderBy(r => r.Size);

                // STEP 4: Display results
                Console.WriteLine("=== Serialization Size Ranking ===\n");

                foreach (var result in ranked)
                {
                    Console.WriteLine($"{result.Format}: {result.Size} bytes");
                }
            }
            catch (Exception ex)
            {
                // Handles any runtime errors (database, file, serialization)
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }
        }

        // =============================
        // GET DATA FROM DATABASE
        // =============================
        static List<Category> GetCategoriesWithProducts(string connectionString)
        {
            // List to store categories
            var categories = new List<Category>();

            // Open SQL connection
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            // SQL query joins Categories and Products tables
            string query = @"
                SELECT c.CategoryID, c.CategoryName,
                       p.ProductID, p.ProductName
                FROM Categories c
                LEFT JOIN Products p ON c.CategoryID = p.CategoryID
                ORDER BY c.CategoryID";

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            Category? currentCategory = null;
            
            // Read each row from database
            while (reader.Read())
            {
                int categoryId = reader.GetInt32(0);

                // if new category, create object
                if (currentCategory == null || currentCategory.CategoryID != categoryId)
                {
                    currentCategory = new Category
                    {
                        CategoryID = categoryId,
                        CategoryName = reader.GetString(1)
                    };

                    categories.Add(currentCategory);
                }

                // Add product if exists (LEFT JOIN may return null)
                if (!reader.IsDBNull(2))
                {
                    currentCategory.Products.Add(new Product
                    {
                        ProductID = reader.GetInt32(2),
                        ProductName = reader.GetString(3)
                    });
                }
            }

            return categories;
        }

        // =============================
        // JSON SERIALIZATION
        // =============================
        static int SerializeToJson(List<Category> data)
        {
            // Convert object to JSON string
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // Save JSON file
            File.WriteAllText("data.json", json);

            // Return size in bytes
            return Encoding.UTF8.GetByteCount(json);
        }

        // =============================
        // XML SERIALIZATION
        // =============================
        static int SerializeToXml(List<Category> data)
        {
            // Create XML serializer
            var serializer = new XmlSerializer(typeof(List<Category>));

            // Use memory stream to store data temporaril
            using var memoryStream = new MemoryStream();

            // Serialize object into XML format
            serializer.Serialize(memoryStream, data);

            // Save XML file
            File.WriteAllBytes("data.xml", memoryStream.ToArray());

            // Return size in bytes
            return (int)memoryStream.Length;
        }

        // =============================
        // BINARY SERIALIZATION
        // =============================
        static int SerializeToBinary(List<Category> data)
{
    // DataContractSerializer converts object to compact binary-like format
    var serializer = new DataContractSerializer(typeof(List<Category>));

    using var memoryStream = new MemoryStream();

    // Write object into memory stream
    serializer.WriteObject(memoryStream, data);
    
    // Save binary file (still .xml structure but compact)
    File.WriteAllBytes("data-binary.xml", memoryStream.ToArray());

    // Return size in bytes
    return (int)memoryStream.Length;
}
    }
    
}