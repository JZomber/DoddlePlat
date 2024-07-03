using UnityEngine;

namespace Command
{
    public interface ICommand
    {
        void Execute();
    }

    public class PhysicsMovementCommand : ICommand
    {
        private readonly float horizontalSpeed;
        private readonly Rigidbody2D targetRigidbody2D;

        public PhysicsMovementCommand(float p_horizontalSpeed, Rigidbody2D p_targetRigidbody2D)
        {
            horizontalSpeed = p_horizontalSpeed;
            targetRigidbody2D = p_targetRigidbody2D;
        }
        
        public void Execute()
        {
            targetRigidbody2D.velocity = new Vector2(horizontalSpeed, targetRigidbody2D.velocity.y);
        }
    }

    public class PhysicsJumpCommand  : ICommand
    {
        private readonly float jumpForce;
        private readonly Rigidbody2D targetRigidbody2D;


        public PhysicsJumpCommand(float p_jumpForce, Rigidbody2D p_targetRigidbody2D)
        {
            jumpForce = p_jumpForce;
            targetRigidbody2D = p_targetRigidbody2D;
        }

        public void Execute()
        {
            targetRigidbody2D.velocity = new Vector2(targetRigidbody2D.velocity.x, jumpForce);
        }
    }

    public class PhysicsKnockBackCommand : ICommand
    {
        private readonly Vector2 knockBackDirection;
        private readonly Rigidbody2D targetRigidbody2D;

        public PhysicsKnockBackCommand(Vector2 p_knockBackDirection, Rigidbody2D p_targetRigidbody2D)
        {
            knockBackDirection = p_knockBackDirection;
            targetRigidbody2D = p_targetRigidbody2D;
        }

        public void Execute()
        {
            targetRigidbody2D.velocity = knockBackDirection;
        }
    }
}