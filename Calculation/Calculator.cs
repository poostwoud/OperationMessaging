namespace Calculation
{
    //[OperationController("calculator")]
    public sealed class Calculator
    {
        //[OperationMethod("add/{operand1:int}/{operand2:int}")]
        public int Add(int operand1, int operand2)
        {
            return operand1 + operand2;
        }

        public int Subtract(int operand1, int operand2)
        {
            return operand1 - operand2;
        }

        public float Divide(float operand1, float operand2)
        {
            return operand1 / operand2;
        }
    }
}
