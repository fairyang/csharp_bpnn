using System;
using BPnnML.Model;

namespace BPnn
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Col0 = 457.9F,
                Col1 = 469.6F,
                Col2 = 468.9F,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Col3 with predicted Col3 from sample data...\n\n");
            Console.WriteLine($"Col0: {sampleData.Col0}");
            Console.WriteLine($"Col1: {sampleData.Col1}");
            Console.WriteLine($"Col2: {sampleData.Col2}");
            Console.WriteLine($"\n\nPredicted Col3: {predictionResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}
