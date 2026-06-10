using System.Reflection.Emit;

namespace CompareSkills;

public static class ReadableOpCodes
{
    /// <summary>
    /// Loads local variable 0 onto the stack.
    /// </summary>
    public static OpCode LoadLocalVariable0 => OpCodes.Ldloc_0;

    /// <summary>
    /// Loads local variable 1 onto the stack.
    /// </summary>
    public static OpCode LoadLocalVariable1 => OpCodes.Ldloc_1;

    /// <summary>
    /// Loads local variable 2 onto the stack.
    /// </summary>
    public static OpCode LoadLocalVariable2 => OpCodes.Ldloc_2;

    /// <summary>
    /// Loads local variable 3 onto the stack.
    /// </summary>
    public static OpCode LoadLocalVariable3 => OpCodes.Ldloc_3;

    /// <summary>
    /// Stores the top of the stack into local variable 0.
    /// </summary>
    public static OpCode StoreLocalVariable0 => OpCodes.Stloc_0;

    /// <summary>
    /// Stores the top of the stack into local variable 1.
    /// </summary>
    public static OpCode StoreLocalVariable1 => OpCodes.Stloc_1;

    /// <summary>
    /// Stores the top of the stack into local variable 2.
    /// </summary>
    public static OpCode StoreLocalVariable2 => OpCodes.Stloc_2;

    /// <summary>
    /// Stores the top of the stack into local variable 3.
    /// </summary>
    public static OpCode StoreLocalVariable3 => OpCodes.Stloc_3;

    /// <summary>
    /// In an instanced method, this is "this". In a static method, this is the first parameter.
    /// </summary>
    public static OpCode LoadArgument0 => OpCodes.Ldarg_0;

    /// <summary>
    /// In an instanced method, this is the first parameter. In a static method, this is the second parameter.
    /// </summary>
    public static OpCode LoadArgument1 => OpCodes.Ldarg_1;

    /// <summary>
    /// In an instanced method, this is the second parameter. In a static method, this is the third parameter.
    /// </summary>
    public static OpCode LoadArgument2 => OpCodes.Ldarg_2;

    /// <summary>
    /// In an instanced method, this is the third parameter. In a static method, this is the fourth parameter.
    /// </summary>
    public static OpCode LoadArgument3 => OpCodes.Ldarg_3;

    /// <summary>
    /// Loads the value of a field from the object on the stack. Requires a FieldInfo operand.
    /// </summary>
    public static OpCode LoadField => OpCodes.Ldfld;

    /// <summary>
    /// Stores a value into a field on the object on the stack. Requires a FieldInfo operand.
    /// </summary>
    public static OpCode StoreField => OpCodes.Stfld;

    /// <summary>
    /// Loads the value of a static field. Requires a FieldInfo operand.
    /// </summary>
    public static OpCode LoadStaticField => OpCodes.Ldsfld;

    /// <summary>
    /// Stores a value into a static field. Requires a FieldInfo operand.
    /// </summary>
    public static OpCode StoreStaticField => OpCodes.Stsfld;

    /// <summary>
    /// Calls a static method or non-virtual instance method. Requires a MethodInfo operand.
    /// </summary>
    public static OpCode CallMethod => OpCodes.Call;

    /// <summary>
    /// Calls a virtual or interface method. Requires a MethodInfo operand.
    /// </summary>
    public static OpCode CallVirtualMethod => OpCodes.Callvirt;

    /// <summary>
    /// Loads the integer constant 0 onto the stack.
    /// </summary>
    public static OpCode LoadInt0 => OpCodes.Ldc_I4_0;

    /// <summary>
    /// Loads the integer constant 1 onto the stack.
    /// </summary>
    public static OpCode LoadInt1 => OpCodes.Ldc_I4_1;

    /// <summary>
    /// Loads the integer constant -1 onto the stack.
    /// </summary>
    public static OpCode LoadIntMinus1 => OpCodes.Ldc_I4_M1;

    /// <summary>
    /// Loads an integer constant onto the stack. Requires an int operand.
    /// </summary>
    public static OpCode LoadInt => OpCodes.Ldc_I4;

    /// <summary>
    /// Loads a string constant onto the stack. Requires a string operand.
    /// </summary>
    public static OpCode LoadString => OpCodes.Ldstr;

    /// <summary>
    /// Duplicates the top value on the stack.
    /// </summary>
    public static OpCode Duplicate => OpCodes.Dup;

    /// <summary>
    /// Removes the top value from the stack.
    /// </summary>
    public static OpCode Pop => OpCodes.Pop;

    /// <summary>
    /// Pops two values off the stack, adds them, and pushes the result.
    /// </summary>
    public static OpCode Add => OpCodes.Add;

    /// <summary>
    /// Pops two values off the stack, subtracts the second from the first, and pushes the result.
    /// </summary>
    public static OpCode Subtract => OpCodes.Sub;

    /// <summary>
    /// Pops two values off the stack, multiplies them, and pushes the result.
    /// </summary>
    public static OpCode Multiply => OpCodes.Mul;

    /// <summary>
    /// Pops two values off the stack, divides the first by the second, and pushes the result.
    /// </summary>
    public static OpCode Divide => OpCodes.Div;

    /// <summary>
    /// Returns from the current method.
    /// </summary>
    public static OpCode Return => OpCodes.Ret;

    /// <summary>
    /// Unconditional branch/goto. Requires a Label operand.
    /// </summary>
    public static OpCode Jump => OpCodes.Br;

    /// <summary>
    /// Branch if the top of the stack is true. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfTrue => OpCodes.Brtrue;

    /// <summary>
    /// Branch if the top of the stack is false. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfFalse => OpCodes.Brfalse;

    /// <summary>
    /// Branch if two values on the stack are equal. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfEqual => OpCodes.Beq;

    /// <summary>
    /// Branch if two values on the stack are not equal. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfNotEqual => OpCodes.Bne_Un;

    /// <summary>
    /// Branch if the first value is greater than the second. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfGreaterThan => OpCodes.Bgt;

    /// <summary>
    /// Branch if the first value is greater than or equal to the second. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfGreaterThanOrEqual => OpCodes.Bge;

    /// <summary>
    /// Branch if the first value is less than the second. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfLessThan => OpCodes.Blt;

    /// <summary>
    /// Branch if the first value is less than or equal to the second. Requires a Label operand.
    /// </summary>
    public static OpCode JumpIfLessThanOrEqual => OpCodes.Ble;
}
